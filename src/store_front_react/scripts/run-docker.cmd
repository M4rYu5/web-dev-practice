 cd ..

 docker stop store-front-react
 docker rm store-front-react
 docker rmi store-front-react

 docker build -t store-front-react .
 docker run -d --name store-front-react -p 3002:80 store-front-react
 
 pause