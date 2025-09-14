FROM node:16.18.0-alpine
RUN mkdir -p /usr/src/nuxt-app
RUN mkdir -p /usr/src/Core.Library
WORKDIR /usr/src/nuxt-app

COPY ./Core.Builder /usr/src/nuxt-app/
COPY ./Core.Library /usr/src/Core.Library

ENV NUXT_HOST=0.0.0.0
ENV NUXT_PORT=3000
#ENV API_BASE_URL= # WILL DEFAULT TO /api
ENV NUXT_ENV_GOOGLE_MAPS_KEY=AIzaSyCIKQ94Rk9oaxIDpt83U7XzKYebFcGBAoE
ENV NUXT_ENV_GOOGLE_RECAPTCHA_SITE_KEY=6LdhkRklAAAAACikikgABh155ITdV4-svXxa_l0M
ENV API_NODE_FUNCTIONS=https://nyl-function-staging.azurewebsites.net/api
ENV API_NODE_CODE=a526yzuAp/jr40unEBeyS8ZGE32dmy/6aPRrOX5w8YP/6LXrMy3d4w==

RUN yarn install
RUN yarn build

EXPOSE 3000

CMD [ "yarn", "start" ]