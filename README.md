# itb-api

[API](https://insttgbot-api.herokuapp.com/)

## Build

#### Build docker image ready for deployment using `Docker`:

```
    docker build -t itb-api .
```

#### Login to `Heroku` using `Heroku CLI`:

```
    heroku container:login
```

#### Push docker image to `Heroku`:

```
    heroku container:push --app=insttgbot-api web
```

#### Start pushed docker image on `Heroku`:

```
    heroku container:release --app=insttgbot-api web
```