#! /bin/bash

if [ ! -f /data/private_key.pem ]; then
  openssl req -new -newkey rsa:4096 -nodes -keyout signing_key.key -out public_key.csr -subj "/C=UK/CN=local"
  openssl x509 -req -sha512 -days 365 -in public_key.csr -signkey signing_key.key -out private_key.pem

  cp private_key.pem /data/private_key.pem
  cp signing_key.key /data/signing_key.key
fi

./Effuse.SSO.Local