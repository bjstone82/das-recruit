print("Start adding/updating LiveVacancy queryViews with default applicationMethod.");

var query = {
        "type": "LiveVacancy",
        "applicationMethod": { $exists: false }
    },
    batchUpdateLimit = 500,
    passThrough = 1;

var maxLoops = Math.ceil(db.queryViews.find().count(query) / batchUpdateLimit);

if (maxLoops === 0) {
    maxLoops = 1;
}

do {
    var matchedDocs = db.queryViews.find(query).limit(batchUpdateLimit).sort({ "lastUpdated": 1 });

    print("Found " + matchedDocs.count() + " document(s) to operate on in passThrough " + passThrough + " of " + maxLoops + ".");

    while (matchedDocs.hasNext()) {
        var doc = matchedDocs.next();

        var writeResult = db.queryViews.update({
            "_id": doc._id,
            "applicationMethod": { $exists: false }
        }, {
            $set: { "applicationMethod": "ThroughExternalApplicationSite" }
        }, {
            upsert: false
        });

        if (writeResult.hasWriteConcernError()) {
            printjson(writeResult.writeConcernError);
            quit(14);
        }

        print("Updated document '" + doc._id + "' with applicationMethod: ThroughExternalApplicationSite.");
    }

    passThrough++;
}
while (passThrough <= maxLoops && db.queryViews.find().count(query) > 0);

print("Finished adding/updating LiveVacancy queryViews with default applicationMethod.");