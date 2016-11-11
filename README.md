# popsicle
Sitecore Marketing Automation Framework for quickly integrating/syncing 
Sitecore xDb with a Marketing Automation Platform

## Features (proposed)

### Event Tracking
 - Triggered Goals sync
 - Triggered Outcomes sync

### Forms

- xDb Autofill Endpoints
- Form Sync
- Rich Text form selector
- Markup Generator for Forms

### Campaigns

- Campaign Sync

### xDb

- xProfile Sync

## Implemented

### Tracking
Goals and Outcomes triggered for a Contact within xDb can be sent as an Entity or an Object to the Marketing Automation platform. Several 
pipelines and hooks are available to hook into:

- Build Tracking Event (ma.buildTrackingEvent) - Maps data from a Page Event to a Tracking Event
- Build Tracking Outcome (ma.buildTrackingOutcome) - Maps data from an Outcome to a Tracking Event
- Filter Page Events (ma.filterPageEvents) - Filters page events before building a Tracking Event
- Filter Outcomes (ma.filterOutcomes) - Filters outcomes before building a Tracking Event
- Flush Tracking Events (ma.flushTrackingEvents) - Sends events to Job Manager, Queue, Platform, etc

### Ingestion

xProfile Sync with Marketing Automation on session start.

Pipeline hook:

- Ingestion Pipeline (ma.ingestion) - Runs configured Hydrator

Additional Hooks and Extension Points:

- IHydrator - Responsible for taking an object that exists in memory (xProfile Facet) and populating the object with data from the Marketing Automation platform
  - ObjectHydratorFromJson is part of the framework to hydrate from a Json Stream
- IDataReader - Responsible for getting the stream of data from the Marketing Automation platform
  - JsonDataReader is part of the framework to read data from a Json (API)
- IHydratorResolver - Responsible for resolving the hydrator with the Object that needs to be hydrated
- IIngestionManager - Responsible for resolving combining the IDataReader and IHydrator to ingest

#### Configuration 

Default Ingestion Configuration

```xml
<?xml version="1.0"?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <ingestion>
      <dataReader type="KKings.Foundation.Popsicle.xDb.Ingest.DataReader.DefaultDataReader, KKings.Foundation.Popsicle" />
      <hydratorResolver type="KKings.Foundation.Popsicle.xDb.Ingest.Resolvers.DefaultHydratorResolver, KKings.Foundation.Popsicle">
        <FacetName>Personal</FacetName>
        <FacetTypeName>Sitecore.Analytics.Model.Entities.IContactPersonalInfo, Sitecore.Analytics.Model</FacetTypeName>
        <HydratorTypeName>KKings.Foundation.Popsicle.xDb.Ingest.Hydrators.ObjectHydratorFromJson, KKings.Foundation.Popsicle</HydratorTypeName>
      </hydratorResolver>
      
      <ingestionManager type="KKings.Foundation.Popsicle.xDb.Ingest.IngestionManager,KKings.Foundation.Popsicle">
        <param name="reader" ref="ingestion/dataReader" />
        <resolvers name="resolvers" hint="list">
          <resolver ref="ingestion/hydratorResolver" singleInstance="true"/>
        </resolvers>
      </ingestionManager>
    </ingestion>
  </sitecore>
</configuration>
```

Multiple Resolvers can be used to hydrate different Facets on the Contact Record. Add Resolvers or references to the ingestion/ingestionManager/resolvers node.

### Forms

The following features have been implemented:

- Token Expander for Injecting markup, the Marketing Form, within Rich-Text Fields 
- Centralized Markup Generator for generating Forms

Override the Default MarkupGenerator by adding a class that imlements the IMarkupGenerator interface. Then patch in the replacement:

```xml
<?xml version="1.0"?>
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <forms>
      <generator type="[Namespace, Class]" singleInstance="true" />
    </forms>
  </sitecore>
</configuration>
```

## In-Progress

Continuation of forms implementation

