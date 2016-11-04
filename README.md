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
- Conditions / Actions

## In-Progress
Goals and Outcomes triggered for a Contact within xDb can be sent as an Entity or an Object to the Marketing Automation platform. Several 
pipelines and hooks are available to hook into:

- Build Tracking Event (ma.buildTrackingEvent) - Maps data from a Page Event to a Tracking Event
- Build Tracking Outcome (ma.buildTrackingOutcome) - Maps data from an Outcome to a Tracking Event
- Filter Page Events (ma.filterPageEvents) - Filters page events before building a Tracking Event
- Filter Outcomes (ma.filterOutcomes) - Filters outcomes before building a Tracking Event
- Flush Tracking Events (ma.flushTrackingEvents) - Sends events to Job Manager, Queue, Platform, etc
