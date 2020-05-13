# SharepointLookup
Biztalk pipeline component to query a Sharepoint list using a key from promoted property.
To read more on how the query is done see this component: https://github.com/BizTalkComponents/LookupUtility 

## Properties
| Property         | Type   | Description                                                                                                |
|------------------|--------|------------------------------------------------------------------------------------------------------------|
| Disabled         | bool   | Disables the component. Default value: false                                                               |
| Promote Property | bool   | Specifies whether the property should be promoted or just written to the context. Default value: false     |
| Throw Exception  | bool   | Throw exception if key or sharepoint list were not found. Default value: false                             |
| Property Path    | string | The property Path to use as key to query the sharepoint list, i.e. http//:examplenamespace#Myproperty      |
| Destination Path | string | The property Path to where the returned value will be promoted to, i.e. http//:examplenamespace#Myproperty |
| List Name        | string | The name of the Sharepoint list to query                                                                   |
