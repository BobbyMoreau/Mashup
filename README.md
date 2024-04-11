# Mashup - REST API

## Problem description
This REST API consumes and packages information from four different APIs:
- **MusicBrainz**
- **Wikidata**
- **Wikipedia**
- **Cover Art Archive**

## Installation
1. Extract the ZIP file.
2. Open the solution file (Mashup.sln).
3. When the project is open, press run in your IDE or you can do it in the terminal: navigate to the project directory and run 'dotnet build' then 'dotnet run'.

## Use the API
When you have built and started the API, you can use it through Postman or similar program. Or if you want, you can run it through Swagger (a common UI for API documentation).

The API needs an id as a parameter to find an Artist.

## Test with these MBIDs if you like
- 13655113-cd16-4b43-9dca-cadbbf26ee05
- d18a2838-05fa-4e4e-bbb5-284a4cfb13f5

You will get an answer in JSON.
The response looks like this:

Example output:
```json
{
    "mbid": "01d3c51b-9b98-418a-8d8e-37f6fab59d8c",
    "description": "Sufjan Stevens ( SOOF-yahn; born July 1, 1975) is an American singer, songwriter, and multi-instrumentalist. He has released ten solo studio albums and multiple collaborative albums with other artists. Stevens has received Grammy and Academy Award nominations.\nHis debut album, A Sun Came, was released in 2000 on the Asthmatic Kitty label, which he co-founded with his stepfather. He received wide recognition for his 2005 album Illinois, which hit number one on the Billboard Top Heatseekers chart, and for the single \"Chicago\" from that album. Stevens later contributed to the soundtrack of the 2017 film Call Me by Your Name. He received an Academy Award nomination for Best Original Song and a Grammy nomination for Best Song Written for Visual Media for the soundtrack's lead single, \"Mystery of Love\".\nStevens has released albums of varying styles, from the electronica of The Age of Adz and the lo-fi folk of Seven Swans to the symphonic instrumentation of Illinois and Christmas-themed Songs for Christmas. He employs various instruments, often playing many of them himself on the same recording. Stevens' music is also known for exploring various themes, particularly religion and spirituality. Stevens' tenth and most recent studio album, Javelin, was released in October 2023.",
    "albums": [
        {
            "title": "A Sun Came",
            "id": "8cf9d337-5074-37fc-aec8-cda21ce959c5",
            "image": "http://coverartarchive.org/release/0742d65b-a013-40f9-a5dd-7b3b8649b527/29588979947.jpg"
        },
        {
            "title": "Enjoy Your Rabbit",
            "id": "a59571bd-1930-3bda-b335-aa91a3b0ff23",
            "image": "http://coverartarchive.org/release/dab7d7c9-2830-4acc-9534-72dbf1f022eb/2655230441.jpg"
        },
        {
            "title": "Michigan",
            "id": "704a628d-949d-3799-b56c-6eb171901d81",
            "image": "http://coverartarchive.org/release/966d3e48-5b17-4ab8-aab4-09dfce55de53/35767939121.jpg"
        }, ... more albums...
}
```

## Room for improvements
- We would need to take care of client-secret and client-id better. They are confidential and should be kept secret. This does not suit a shared repository online.
- To have an API that is more useful, the API needs more endpoints. So if we were to keep working on this, more endpoints would be something to prioritize. 
- If the API would grow, we would add more objects to easier handle the data from all the API requests we are doing. The way we handle the JSON is not optimal if we would want more info out of the requests. 
- The API is rather slow, we would need to look at how to handle big Artists with a lot of albums. 


# External API documentation
## MusicBrainz
- Documentation: [MusicBrainz API Documentation](https://musicbrainz.org/doc/Development/XML_Web_Service/Version_2)
- URL: https://musicbrainz.org/ws/2
- Example: [MusicBrainz Example](https://musicbrainz.org/ws/2/artist/5b11f4ce-a62d-471e-81fc-a69a8278c7da?&fmt=json&inc=url-rels+release-groups)

## Wikidata
- Documentation: [Wikidata API Documentation](https://www.wikidata.org/w/api.php)
- URL: https://www.wikidata.org/w/api.php
- Example: [Wikidata Example](https://www.wikidata.org/w/api.php?action=wbgetentities&ids=Q11649&format=json&props=sitelinks)

## Wikipedia
- Documentation: [Wikipedia API Documentation](https://www.mediawiki.org/wiki/API:Main_page)
- URL: https://en.wikipedia.org/w/api.php
- Example: [Wikipedia Example](https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles=Nirvana_(band))

## Cover Art Archive
- Documentation: [Cover Art Archive API Documentation](https://wiki.musicbrainz.org/Cover_Art_Archive/API)
- URL: https://coverartarchive.org/
- Example: [Cover Art Archive Example](https://coverartarchive.org/release-group/1b022e01-4da6-387b-8658-8678046e4cef)
