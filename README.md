# EventsStreamingApplication
Application for streaming and analyzing events, using RabbitMQ

Project: 
ApiConnector: 
  - subscribes external api (currently configured in ApiConnectorConsoleApp as "http://stream.meetup.com/2/rsvps")
  - procuces messages, adds to rawEventsQueue

EventTransformerWorker: 
  - consumes messages from rawEventsQueue
  - parses and transforms messages into transformedEventModel
  - groups topic names from events, stores number for each of them
  - can have many instances (to parallellize messages transform)
  - sends calculations to queue (TODO!)
  
 EventsWebDashboard:
   - consumes partial results from event transformer workers queue (TODO!)
   - calculates final counts per each topic name
   - returns (on user demand - access to controller), current calculation values (for example top 10 topics)
