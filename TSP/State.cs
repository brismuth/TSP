using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TSP
{
	class State:IComparable<State>
	{
		public CostMatrix costMatrix;
		public int lowerBound;
		public int depth;
		public Dictionary<City, City> edges;
		
		public State(CostMatrix costMatrix, int lowerBound, int depth, Dictionary<City, City> edges)
		{
			this.costMatrix = costMatrix;
			this.lowerBound = lowerBound;
			this.depth = depth;
			this.edges = edges;
		}
		
		public ArrayList getRoute()
		{
			ArrayList route = new ArrayList();
			City startCity = Enumerable.ToList(edges.Keys)[0];
			City curCity = startCity;
			do
			{
				route.Add(curCity);
				curCity = edges[curCity];
			}
			while (curCity != startCity);
			
			return route;
		}
		
		public List<State> GetChildren()
		{
			List<State> children = new List<State>();
			Tuple<int,int> IJ = this.costMatrix.GetRecommendedIJ();
			children.Add(this.costMatrix.IncludeEdge(IJ.Item1, IJ.Item2));
			children.Add(this.costMatrix.IncludeEdge(IJ.Item1, IJ.Item2));
		}
		
		public int CompareTo(State state)
		{
			// todo
			return state.lowerBound.CompareTo(lowerBound);
		}
	}
}

