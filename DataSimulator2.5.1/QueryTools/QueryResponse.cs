using System;
using System.Collections.Generic;

namespace EdpSimulator.QueryTools
{
    public class QueryResponse
    {
        public QueryResponse(Tuple<List<string>,int> resultsAndTime)
            => this.ResultsAndTime = resultsAndTime;
        public QueryResponse(List<string> queryResults, int queryTime)    // overloaded constructor
            => this.ResultsAndTime = new Tuple<List<string>,int>(queryResults, queryTime);
        public Tuple<List<string>,int> ResultsAndTime {get;set;}

        public int QueryTime => ResultsAndTime.Item2;
        public List<String> QueryResults => ResultsAndTime.Item1;
        public string FirstResult => ResultsAndTime.Item1[0];
    }
}