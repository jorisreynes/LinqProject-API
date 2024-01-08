# LinqProject-API

## The project

The purpose of this project is to visualize your chess game stats, the application is built with a C# .NET 6.0 backend API and a frontend made with Angular,

You can find the LinqProject-Angular repository here : https://github.com/jorisreynes/LinqProject-Angular

## How it works

You need to download the .pgn file from Chess.com, you can upload it with the Angular front (or with Postman if you prefer)

Once the file is sent on the backend, you can see your winrate and you can filter the games with the Opening, the color, and the End of game

For example you can see your winrate for the Scottish opening, or compare your winrate with white or black, or see your winrate because of checkmate or time (interesting if you play blitz)

## Other

I also created the same API in Java to compare, you can find the repo here : https://github.com/jorisreynes/ChessResultAnalyzerJava

And I created the same project with the MERN stack to compare, you can find the repo here : https://github.com/jorisreynes/mern

## How to install it :

````
git clone https://github.com/jorisreynes/LinqProject-API.git
````

````
cd LinqProject-API
````

- Open the solution with Visual Studio (or another IDE if you prefer)

- Launch the project

You will have access to the Swagger interface

![LinqProject-API](APIScreenshot.jpg)
