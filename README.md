# Troupe Chat

## Overview

Troupe Chat is a real-time chat application designed to connect students and self-taught professionals through vibrant, interest-based communities. The goal is to create a platform where people can find and engage with others who share their passions, whether it’s anime, programming, literature, fitness, or entrepreneurship.

The software provides instant messaging combined with community discovery, allowing users to create or join “Troupes” — focused groups where members can share ideas, collaborate, and build meaningful connections. Unlike traditional platforms that are either too broad or static, Troupe Chat fills the gap by offering tailored spaces for learners and creators to organize discussions, coordinate projects, and grow their networks.

The purpose of building Troupe Chat is not just to make another chat app, but to design a flexible and empowering platform where users can shape their communities. By providing tools for real-time messaging, profile management, and interest-driven community creation, Troupe Chat becomes a foundation for building networks, launching ideas, and supporting collaboration in a modern and responsive environment.

📺 **YouTube Demonstration**: [Demo Link Here]

---

## Features

- **User Account Creation & Authentication**

  - Register and log in using email and password
  - Authentication ensures secure access

- **Real-Time Messaging**

  - 1-on-1 chats or group discussions
  - Messages appear instantly without page reload

- **Interest-Based Communities (“Troupes”)**

  - Users can create and join communities around shared interests
  - Smaller project-specific groups can exist within communities

- **Community Discovery**

  - Search and explore communities by subject or interest
  - View and join active groups

- **User Profiles**

  - Edit display name, profile picture, and personal info
  - View other users’ profiles within communities

- **Responsive UI**
  - Works across desktops, tablets, and mobile devices

### Stretch Features (Future Work)

- Community management tools (admins, membership management)
- Push notifications for new messages and invites
- Progressive Web App (PWA) support with offline capability

---

## Development Environment

- **Framework & Language**:

  - [.NET](https://dotnet.microsoft.com/) with **Blazor** for modern, component-driven UI
  - **C#** as the main programming language

- **Real-time Communication**:

  - **SignalR and WebSockets** for instant messaging

- **Backend & Storage**:

  - **Supabase** for authentication, relational database, and optional cloud storage

- **Security**:

  - HTTPS, password hashing, session management, and role-based access control

- **Hosting**:

  - Deployable on **Render** or **Azure**

---

# Useful Websites

{Make a list of websites that you found helpful in this project}

- [SignalR Tutorial](https://learn.microsoft.com/es-es/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-9.0&tabs=visual-studio-code)
- [Blazor ChatApp Tutorial](https://learn.microsoft.com/en-us/azure/azure-signalr/signalr-tutorial-build-blazor-server-chat-app)
