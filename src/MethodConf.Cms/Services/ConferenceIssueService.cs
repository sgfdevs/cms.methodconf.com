using AutoMapper;
using FluentResults;
using MethodConf.Cms.Domain;
using MethodConf.Cms.Domain.Errors;
using MethodConf.Cms.Dtos;
using MethodConf.Cms.Infrastructure;
using MethodConf.Cms.Services.Interfaces;
using Microsoft.Extensions.Options;
using Razor.Templating.Core;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace MethodConf.Cms.Services;

public class ConferenceIssueService(
    IPublishedContentQuery publishedContentQuery,
    AppDbContext dbContext,
    IEmailSender emailSender,
    IRazorTemplateEngine razorEngine,
    IOptions<GlobalSettings> globalSettings,
    IMapper mapper) : IConferenceIssueService
{
    private const string OrganizerEmailSubject = "New Issue Reported for Method Conf";
    private const string ReporterEmailSubject = "Your Issue Has Been Recieved";
    private const string OrganizerEmail = "info@methodconf.com";
    public async Task<Result<IssueWithResponse>> CreateIssue(Guid conferenceId, CreateIssue createIssue)
    {
        if (publishedContentQuery.Content(conferenceId) is not Conference conference)
        {
            return Result.Fail(new InvalidEntityIdError(conferenceId.ToString()));
        }

        var newIssue = mapper.Map<Issue>(createIssue);
        newIssue.ConferenceId = conference.Key;

        dbContext.Add(newIssue);
        await dbContext.SaveChangesAsync();

        if (newIssue.Email is { } email && !string.IsNullOrWhiteSpace(email))
        {
            try
            {
                await SendReporterEmail(email, newIssue);
            }
            catch
            {
                await SendOrganizerEmail(newIssue);
            }
        }
        else
        {
            await SendOrganizerEmail(newIssue);
        }

        var issueWithResponse = mapper.Map<IssueWithResponse>(newIssue);
        issueWithResponse.ResponseMarkup = await GetResponseMessage(newIssue) ?? "";

        return issueWithResponse;
    }

    private async Task SendOrganizerEmail(Issue issue)
    {
        var vm = mapper.Map<NewIssueEmailViewModel>(issue);
        vm.Title = OrganizerEmailSubject;

        var content = await razorEngine.RenderAsync("~/Views/Templates/NewIssueOrganizerEmail.cshtml", vm);

        var message = new EmailMessage(
            from: globalSettings.Value.Smtp?.From,
            to: OrganizerEmail,
            subject: OrganizerEmailSubject,
            body: content,
            isBodyHtml: true);

        await emailSender.SendAsync(message, emailType: "Organizer Message");

    }

    private async Task SendReporterEmail(string reporterEmail, Issue issue)
    {
        var vm = mapper.Map<NewIssueEmailViewModel>(issue);
        vm.Title = ReporterEmailSubject;
        var content = await razorEngine.RenderAsync("~/Views/Templates/NewIssueReporterEmail.cshtml", vm);

        var message = new EmailMessage(
            from: globalSettings.Value.Smtp?.From,
            to: [reporterEmail],
            cc: [OrganizerEmail],
            bcc: null,
            replyTo: [OrganizerEmail],
            subject: ReporterEmailSubject,
            body: content,
            isBodyHtml: true,
            attachments: null);

        await emailSender.SendAsync(message, emailType: "Reporter Message");
    }

    public async Task<string?> GetResponseMessage(Issue issue)
    {
        var vm = mapper.Map<NewIssueAppResponseViewModel>(issue);
        return await razorEngine.RenderAsync("~/Views/Templates/NewIssueAppResponse.cshtml", vm);
    }
}
