FROM composer:2 as vendor
COPY composer.json composer.json
COPY composer.lock composer.lock
RUN composer install --ignore-platform-reqs --no-interaction --prefer-dist

FROM craftcms/cli:8.2

COPY --chown=www-data:www-data --from=vendor /app/vendor/ /app/vendor/

COPY --chown=www-data:www-data . .
