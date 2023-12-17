docker rm -f DevJobSchedulerService
docker build . -t dev-job-scheduler-service && ^
docker run -d --name DevJobSchedulerService -p 49057:80 ^
--env-file ./../../secrets/secrets.dev.list ^
-e ASPNETCORE_ENVIRONMENT=DockerDev ^
-it dev-job-scheduler-service
echo finish dev-job-scheduler-service
docker image prune -f
pause
