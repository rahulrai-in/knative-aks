rem deploy orchestrator
docker build -t rahulrai/hello-orchestrator ./hello-orchestrator/
docker push rahulrai/hello-orchestrator
rem test the image
docker run --name kn-hello-orchestrator -e StorageConnectionString=DefaultEndpointsProtocol=https;AccountName={ACC NAME};AccountKey={ACC KEY};EndpointSuffix=core.windows.net -e TaskHubName=DTFHub3 -e K_SINK=http://webhook.site/{USED_FOR_STANDALONE_TESTS} rahulrai/hello-orchestrator
pause
rem pause
docker container rm -f kn-hello-orchestrator

rem deploy events
docker build -t rahulrai/hello-events ./Hello-Events/
docker push rahulrai/hello-events
rem test the image
docker run --rm -p 8080:80 --name kn-hello-events rahulrai/hello-events
pause
docker container rm -f kn-hello-events