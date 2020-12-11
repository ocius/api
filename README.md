# USV API

**Extract** (AWS Lambda)

XmlToJson (Cloud Watch cron)  --> TimeSeriesDroneData Table --> Triggers RawDataToClientData (lambda) --> saves to drone locations and drone sensors into DynamoDB table


**Load** (AWS Lambda/running process)

GetCameraNames (Cloud Watch cron) --> saves to CameraNamesTable --> GetCameraImages (on schedule) --> will download image from ocius and then upload to S3 and put s3 url into table --> CameraImageUrls stores s3 urls

**Serve** (API Gateway)

GetAll


## Services
- XMLToJson (for more frequent requests)
- XMLToJsonLambda (Lambda Function)
  - Downloads from mav_status active USVs names and inserts into database
  - See ExampleMavStatus.xml for template xml
- RawDatatoClientData (DynamoDB Trigger)
  - Captures insertions into the DB
  - Inserts USV the latest camera image with the drone sensor data
- GetCameraImages
  - Downloads an image, saves the image into a s3 bucket, inserts the url to the image into DynamoDB
- GetCameraNames
  - Fetches the names and ids of active USVs
  - see ExampleListCameras.xml

## Endpoints
### GetAll (API Gateway)
Queries the database for USVS sensor data (locations, battery, etc)
  - Function.cs: The main body of the program, returns the type of API response (timespan data/live sensor data) based on the GET request paramters
  - APIresponse.cs: A APIGateway wrapper for database interactions. Defines the desired resources from the database. Also supplies HTTP response headers that go back to the client.
  - Database.cs: A centralised interface for requesting resources from the database. This will translate requests into concrete Command/Query implementations following the CQRS pattern (https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)

## Development
For linux users
```
cd docker
sudo sh build.sh
sudo sh run.sh /path/to/api/repository
```

## FAQ

Q: Why is data only coming in every 2 minutes

A: It's an AWS lambda limitation, use an actual process to if you need more frequent data


Q: Are things breaking?

A: Happens often when the XML API changes, see logs for RawDataToClient
