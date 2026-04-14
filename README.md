# AIChatBot

A backend API for an AI-powered chatbot built with **ASP.NET Core 8**.  
The project exposes chat endpoints and gives the model access to helper functions for **weather forecasts**, **currency conversion**, and **time zone lookups**.

## Overview

This repository contains the server-side part of the project. The API:

- accepts chat messages from a frontend client
- keeps a running chat history in memory
- sends prompts to OpenAI's Chat Completions API
- supports function calling for external data tasks
- exposes standalone endpoints for weather, currency exchange, and time zone lookups
- allows the current chat session to be reset

The backend is configured to allow CORS requests from `http://localhost:4200`, which matches the Angular frontend in the companion UI repository.

## Features

- **AI chat endpoint** using OpenAI chat completions
- **Function calling support** for:
  - weather forecasts
  - currency conversion
  - time and date by IANA time zone
- **Separate REST endpoints** for helper services
- **Swagger/OpenAPI** enabled in development
- **CORS policy** for local Angular development
- **Simple in-memory conversation state** with reset support

## Tech Stack

- **.NET 8 / ASP.NET Core Web API**
- **C#**
- **Swagger / Swashbuckle**
- **Newtonsoft.Json**
- **HttpClient** for external API calls

## How It Works

When a user sends a message to the chat endpoint:

1. the message is added to the in-memory conversation history
2. a system prompt is loaded from `TextsForAI/SystemContextAI.txt`
3. the backend sends the conversation to OpenAI
4. if the model requests a function call, the backend executes one of its helper services
5. the helper service result is added back into the conversation
6. the final assistant response is returned to the client

This creates a chatbot that can answer normal questions while also retrieving live external data when needed.

## API Endpoints

### AI Chat

#### `POST /Ai/Message`
Send a user question to the chatbot.

**Request body**
```json
"What is the weather in Stockholm for the next 2 days?"
```

**Response**
```text
A plain text AI response.
```

#### `POST /Ai/ResetChatMessages`
Clears the current in-memory conversation history.

---

### Weather

#### `GET /Weather/forecast?location=Stockholm&days=2`
Returns a weather forecast as formatted text.

---

### Currency

#### `GET /Currency/CurrencyExchange?baseCurrency=USD&diffCurrency=SEK&amount=100`
Returns conversion rate and conversion result.

---

### Time Zone

#### `GET /TimeZone/GetTimeByIanaZone?Iana=Europe/Stockholm`
Returns the current date and time for a given IANA time zone.

## Configuration

The services load secrets and external API URLs from:

```text
Configuration/appsettings.json
```

Create that file before running the project.

### Example `Configuration/appsettings.json`

```json
{
  "OpenAiSecretKey": "your-openai-api-key",
  "chatGPTUrl": "https://api.openai.com/v1/chat/completions",
  "WeatherApiKey": "your-weather-api-key",
  "WeatherUrl": "https://api.weatherapi.com/v1/forecast.json?key=",
  "ExchangeApiKey": "your-exchange-rate-api-key",
  "ExchangeUrl": "https://v6.exchangerate-api.com/v6",
  "TimeZoneUrl": "https://timeapi.io/api"
}
```

## Prerequisites

Before you run the project, make sure you have:

- **.NET 8 SDK**
- a valid **OpenAI API key**
- a weather API key
- an exchange-rate API key
- access to the configured time zone API

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Charbel03/AIChatBot.git
cd AIChatBot/AIChatBot
```

### 2. Add configuration

Create:

```text
Configuration/appsettings.json
```

Then add your API keys and URLs.

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Run the API

```bash
dotnet run
```

### 5. Open Swagger

In development, Swagger UI is enabled automatically. Open the local Swagger URL shown in your terminal after startup.

## Frontend Integration

This backend is set up to work with the Angular frontend in:

- `AIChatBotUI`

By default, CORS allows requests from:

```text
http://localhost:4200
```

If your frontend runs on a different origin, update the CORS policy in `Program.cs`.


## Companion Repository

Frontend UI repository:

- `https://github.com/Charbel03/AIChatBotUI`
