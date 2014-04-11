using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace TSP
{
	class State:IComparable<State>
	{
		public CostMatrix costMatrix;
		public double lowerBound;
		public int depth;
		public Dictionary<City, City> edges;
		private City[] cities;
		
		public State(CostMatrix costMatrix, double lowerBound, int depth, Dictionary<City, City> edges, City[] cities)
		{
			this.costMatrix = costMatrix;
			this.lowerBound = lowerBound;
			this.depth = depth;
			this.edges = edges;
			this.cities = cities;
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
			
			CostMatrix cmA = this.costMatrix.IncludeEdge(IJ.Item1, IJ.Item2);
			Dictionary<City, City> edgesA = new Dictionary<City, City>();
			foreach (KeyValuePair<City, City> entry in this.edges) edgesA.Add(entry.Key, entry.Value);
			edgesA.Add(this.cities[IJ.Item1], this.cities[IJ.Item2]);
			State stateA = new State(cmA, cmA.GetBound(), this.depth + 1, edgesA, cities);
			
			CostMatrix cmB = this.costMatrix.ExcludeEdge(IJ.Item1, IJ.Item2);
			Dictionary<City, City> edgesB = new Dictionary<City, City>();
			foreach (KeyValuePair<City, City> entry in this.edges) edgesB.Add(entry.Key, entry.Value);
			State stateB = new State(cmB, cmB.GetBound(), this.depth + 1, edgesB, cities);
			                                              
			children.Add(stateA);
			children.Add(stateB);
			return children;
		}
		
		public int CompareTo(State state)
		{
			// todo
			return state.lowerBound.CompareTo(lowerBound);
		}
	}
}

