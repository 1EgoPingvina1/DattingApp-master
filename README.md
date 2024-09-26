👩‍❤️‍💋‍👨 Tinderclone
A web application presenting a Tinder clone. It allows you to view different people, people with whom you have a match, and there is also a chat module that allows you to have a conversation with selected people. Project written using .NET 6 / Angular 14 combo.

Tools used
Angular 14, HTML, CSS, RxJS, Bootstrap, Typescript - frontend
.NET 6, ASP.NET Core Web API, ASP.NET Core SignalR, ASP.NET Core Identity, Entity Framework Core, SQLite - backend
The entire application has been implemented in accordance with the CQRS - Command and Query Responsibility Segregation pattern, for the implementation of similar segregation it has been used MediatR add-on.

Implemented Things
Authentication based on JWT-tokens
Authorization via roles - in connection with ASP.NET Core Identity
Message box management, live chat with SignalR
Online People Visibility - Using SignalR
Ability to add profile pictures and set the main ones
Admin panel that manages user roles and approves/rejects new user photos.
Page pagination of all users and liked users
Caching on the side of Angular services
Edit a person's entire profile
CQRS + MediatR
Angular Route Guards to counter accidental actions
Angular Interceptors to send queries with the authentication header
And many other :)

How to Install
1️⃣ Download the backend + frontend
2️source code ⃣ Run the API (it will automatically migrate the SQL Server database).
3️⃣ Launch the client site: ng serve

