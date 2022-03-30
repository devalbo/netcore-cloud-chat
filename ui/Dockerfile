FROM node:16.13.2

WORKDIR /usr/src/app

EXPOSE 3000

COPY package-lock.json ./
COPY package.json ./

RUN npm install

COPY . .

