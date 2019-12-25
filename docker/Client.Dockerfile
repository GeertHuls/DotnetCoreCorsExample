FROM nginx:1.16.0-alpine

# the './' context of the client directory is set in the docker-compose file
COPY ./ /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
