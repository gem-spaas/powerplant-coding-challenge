# ----------------------------------------------------------------------
FROM python:3.12.0-alpine3.18 as compile-stage

RUN apk add --no-cache \
    gcc \
    musl-dev \
    linux-headers

COPY ./requirements.txt ./requirements.txt
RUN pip install --prefix=/pyinstall -r requirements.txt 
    
# ----------------------------------------------------------------------
FROM python:3.12.0-alpine3.18 as runtime

LABEL version=1.0-beta \
      description="GEM Energy Production Plan microservice" \
      mantainer="Pablo Cazallas <pablo.cazallas@gmail.com>"

COPY --from=compile-stage /pyinstall /usr/local
WORKDIR /service
COPY . .
RUN rm ./requirements.txt

ENV FLASK_APP=app.py
ENV FLASK_DEBUG=true
ENV FLASK_RUN_HOST=0.0.0.0

CMD [ "flask", "run", "--host=0.0.0.0", "--port=8888" ]
