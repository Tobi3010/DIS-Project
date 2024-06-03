#!/bin/bash
createdb fifty-best
psql -f FiftyBest/Data/dbschema.sql fifty-best
psql -d fifty-best -f Data/all.sql