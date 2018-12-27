# Build node app
FROM node:lts-alpine as build-node

RUN mkdir /app
WORKDIR /app

COPY ./ClientApp/package*.json ./
RUN npm install

COPY ./ClientApp ./
RUN npm run build

# Build dotnet app
FROM microsoft/dotnet:2.2-sdk-alpine AS build-dotnet

RUN mkdir -p /app/publish
WORKDIR /app

COPY Books.csproj .
RUN dotnet restore ./Books.csproj

COPY --from=build-node /app/dist/ /app/ClientApp/dist/
COPY . .

RUN dotnet publish Books.csproj -c Release -r alpine-x64 -o publish -p:SkipNpm=true --self-contained

# Final container
FROM microsoft/dotnet:2.2-runtime-deps-alpine

RUN mkdir /app
WORKDIR /app
COPY --from=build-dotnet /app/publish /app/

ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

CMD [ "/app/Books", "web" ]
