### STAGE 1: Build ###
FROM node:20.12.2-alpine AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --prod

### STAGE 2: Run ###
FROM nginx:1.17.1-alpine
COPY nginx.conf ./nginx.conf
COPY --from=build /usr/src/app/dist/aston-villa-app /usr/share/nginx/html