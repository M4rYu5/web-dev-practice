FROM node:20.8.0 as build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build



FROM debian:12.2-slim

WORKDIR /app

RUN apt-get update && apt-get install -y apache2

COPY --from=build /app/out /var/www/html/
# COPY ./out /var/www/html/

EXPOSE 80

# Enable mod_rewrite
RUN a2enmod rewrite

# Add a custom site configuration that includes the rewrite rules
RUN echo '<VirtualHost *:80>\n\
    DocumentRoot /var/www/html\n\
    <Directory /var/www/html>\n\
        Options Indexes FollowSymLinks MultiViews\n\
        AllowOverride All\n\
        Require all granted\n\
        RewriteEngine on\n\
        RewriteBase /\n\
        RewriteCond %{REQUEST_FILENAME} !-f\n\
        RewriteCond %{REQUEST_FILENAME} !-d\n\
        RewriteRule ^([^/]+)/?$ $1.html [L]\n\
    </Directory>\n\
</VirtualHost>' > /etc/apache2/sites-available/000-default.conf

# Enable the site
RUN a2ensite 000-default.conf

# Run Apache in the foreground
CMD ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]
