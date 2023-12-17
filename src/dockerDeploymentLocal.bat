docker rm -f LocalJobSchedulerService
docker build . -t local-job-scheduler-service && ^
docker run -d --name LocalJobSchedulerService -p 47057:80 ^
--env-file ./../../secrets/secrets.local.list ^
-e ASPNETCORE_ENVIRONMENT=DockerLocal ^
-it local-job-scheduler-service
echo finish local-job-scheduler-service
docker image prune -f
pause
