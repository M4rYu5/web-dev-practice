 cd ..

 docker stop chat_preview
 docker rm chat_preview
 docker rmi chat_preview

 docker build -t chat_preview .
 docker run -d --name chat_preview -p 3001:80 chat_preview
 
 pause