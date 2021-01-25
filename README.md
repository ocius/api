# USV API

## Data Pipeline

XmlToJsonLambda (Cloud Watch cron)  --> TimeSeriesDroneData Table --> Triggers RawDataToClientData (lambda) --> saves to drone locations and drone sensors into DynamoDB table

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
  - Function.cs: The main body of the program. Parses the request into C# data structures. Returns the type of API response (timespan data/live sensor data) based on the GET request paramters
  - APIresponse.cs: An API Gateway wrapper for database interactions. Defines the desired resources from the database. Also supplies HTTP response headers that go back to the client.
  - Database.cs: A centralised interface for requesting resources from the database. This will translate requests into concrete Command/Query implementations following the CQRS pattern (https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)

## Deployment
Deployment of this repo is automated in github workflows here https://github.com/ocius/api/tree/master/.github/workflows

### Stage 1
In stage 1 of deployment, data fetching services are deployed when merged with the `stage-1` branch.
After deploying data fetching services, developers will be able to validate the program by checking the data insertions into DynamoDB.

### Stage 2
In stage 2 of deployment, endpoint services are deployed when merged with the `master` branch.
After deploying endpoint services, developers will be able to validate the program by checking https://api.ocius.com.au/drones

## FAQ

Q: Why is data only coming in every 2 minutes

A: It's an AWS lambda limitation, use an actual process to if you need more frequent data


Q: Are things breaking?

A: Happens often when the XML API changes, see logs for RawDataToClient
