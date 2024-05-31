#!/bin/bash
createdb fifty-best
psql -f FiftyBest/Data/dbschema.sql fifty-best
