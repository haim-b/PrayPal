#!/usr/bin/env bash
#
#
# DECLARE THE APP_CENTER_SECRETS_JSON ENVIRONMENT VARIABLE IN APP CENTER BUILD CONFIGURATION, SET
# TO THE CONTENTS OF YOUR secrets.json FILE

if [ -z "$APP_CENTER_SECRETS_JSON" ]
then
    echo "You need define the APP_CENTER_SECRETS_JSON variable in App Center"
    exit
fi

# This is the path to the secrets.json file, Update '4J\4J\4J' to be the
# correct path to the file relative to the root of your repo
APP_CENTER_SECRETS_JSON_FILE=$APPCENTER_SOURCE_DIRECTORY/4J/4J/secrets.json

echo "Creating secrets.json"
echo "$APP_CENTER_SECRETS_JSON" > $APP_CENTER_SECRETS_JSON_FILE
sed -i -e 's/\\"/'\"'/g' $APP_CENTER_SECRETS_JSON_FILE

echo "File content:"
cat $APP_CENTER_SECRETS_JSON_FILE