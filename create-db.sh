#!/bin/bash
createdb fifty-best
psql -f FiftyBest/Data/DbSchema.sql fifty-best
psql -d fifty-best -f Data/all.sql
