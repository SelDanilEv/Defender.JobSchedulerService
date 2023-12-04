docker rm -f JobSchedulerService
docker build . -t job-scheduler-service && ^
docker run -d --name JobSchedulerService -p 0000:80 ^
--env-file ./../../secrets.list ^
-e ASPNETCORE_ENVIRONMENT=DockerDev ^
-it job-scheduler-service
echo finish job-scheduler-service
pause
