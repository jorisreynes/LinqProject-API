# LinqProject-API

## The project

The purpose of this project is to visualize your chess game stats, the application is built with a C# .NET 6.0 backend API and a frontend made with Angular,

You can find the LinqProject-Angular repository here : https://github.com/jorisreynes/LinqProject-Angular

## How it works

You need to download the .pgn file from Chess.com, you can upload it with the Angular front (or with Postman if you prefer)

Once the file is sent on the backend, you can see your winrate and you can filter the games with the Opening, the color, and the End of game

For example you can see your winrate with the Scotch opening, or compare your winrate with white or black, or see your winrate because of checkmate or time (interesting if you play blitz)

## Other

I also created the same API in Java to compare, you can find the repo here : https://github.com/jorisreynes/ChessResultAnalyzerJava

And I created the same project with the MERN stack to compare, you can find the repo here : https://github.com/jorisreynes/mern

This project is not finished because finally there is a Chess.com API with more data than what we have in the .pgn file, so the backend is not needed anymore, 

There is a new version, frontend only, of this project here : https://github.com/jorisreynes/ChessGameStats

## How to install it :

````
git clone https://github.com/jorisreynes/LinqProject-API.git
````

````
cd LinqProject-API
````

- Open the solution with Visual Studio (or another IDE if you prefer)

- Launch the project

You will have access to the Swagger interface :

![LinqProject-API](APIScreenshot.jpg)

The front :

![LinqProject-API](AngularScreenshot.jpg)
