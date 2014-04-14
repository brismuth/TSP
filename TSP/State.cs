using System;
using System.Collections.Generic;
using System.Collections;

namespace TSP
{
	class State:IComparable<State>
	{
		public CostMatrix costMatrix;
		public double lowerBound;
		public int depth;
		public Dictionary<City, City> edges;
		private City[] cities;
		private ArrayList partialRoute;

		public State(CostMatrix costMatrix, double lowerBound, int depth, Dictionary<City, City> edges, City[] cities)
		{
			this.costMatrix = costMatrix;
			this.lowerBound = lowerBound;
			this.depth = depth;
			this.edges = edges;
			this.cities = cities;
		}

		public State(CostMatrix costMatrix, double lowerBound, int depth, Dictionary<City, City> edges, City[] cities, ArrayList partialRoute)
		{
			this.costMatrix = costMatrix;
			this.lowerBound = lowerBound;
			this.depth = depth;
			this.edges = edges;
			this.cities = cities;
			this.partialRoute = partialRoute;
		}

		public ArrayList getRoute()
		{
			City startCity = cities[0];

			if (partialRoute == null) {
				partialRoute = new ArrayList ();
				partialRoute.Add(startCity);
			}

			City curCity = partialRoute [partialRoute.Count-1] as City;

			while (edges.ContainsKey(curCity))
			{
				if (edges[curCity] == startCity) return partialRoute;
				partialRoute.Add(edges[curCity]);
				curCity = edges [curCity];
			}

			return null;
		}
		
		public List<State> GetChildren()
		{
			List<State> children = new List<State>();
			Tuple<int,int> IJ = this.costMatrix.GetRecommendedIJ();

			if (IJ == null)
				return children;

			CostMatrix cmA = this.costMatrix.IncludeEdge(IJ.Item1, IJ.Item2);
			Dictionary<City, City> edgesA = new Dictionary<City, City>(this.edges);
			edgesA.Add(this.cities[IJ.Item1], this.cities[IJ.Item2]);
			State stateA = new State(cmA, cmA.GetBound(), this.depth + 1, edgesA, cities, partialRoute == null ? null : partialRoute.Clone() as ArrayList);
			
			CostMatrix cmB = this.costMatrix.ExcludeEdge(IJ.Item1, IJ.Item2);
			Dictionary<City, City> edgesB = new Dictionary<City, City>(this.edges);
			State stateB = new State(cmB, cmB.GetBound(), this.depth, edgesB, cities, partialRoute == null ? null : partialRoute.Clone() as ArrayList);
			                                              
			children.Add(stateA);
			children.Add(stateB);
			return children;
		}
		
		public int CompareTo(State state)
		{
			double s = 0.00000000000001;
			double p1 = s * lowerBound + (1-s)*(cities.Length-depth);
			double p2 = s * state.lowerBound + (1-s)*(cities.Length-state.depth);
			return p1.CompareTo(p2);
		}
	}
}

