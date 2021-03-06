using System;

namespace TSP
{
	class CostMatrix
	{
		protected double[,] matrix;
		protected double bound;

		public CostMatrix(City[] cities)
		{
			matrix = new double[cities.Length,cities.Length];

			for (int i = 0; i < cities.Length; i++)
			{
				for (int j = 0; j < cities.Length; j++)
				{
					if (i == j) matrix[i,j] = Double.PositiveInfinity;
					else matrix[i,j] = cities[i].costToGetTo(cities[j]);
				}
			}

			bound = Reduce();
		}
		
		public CostMatrix(double[,] matrix)
		{
			this.matrix = matrix;
		}

		public double GetBound() { return bound; }

		protected double ReduceRows()
		{
			double bound = 0;

			// Consider each row.
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				// Find the lowest value in the row.
				double lowest = Double.PositiveInfinity;
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (matrix[i, j] < lowest) lowest = matrix[i, j];
				}

				// If it's already reduced, move on.
				if (lowest == 0 || Double.IsPositiveInfinity(lowest)) continue;

				// Reduce each cell by the lowest value.
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] -= lowest;
				}

				bound += lowest;
			}

			return bound;
		}


		protected double ReduceColumns()
		{
			double bound = 0;

			// Consider each column.
			for (int j = 0; j < matrix.GetLength(0); j++)
			{
				// Lowest value in the column.
				double lowest = Double.PositiveInfinity;
				for (int i = 0; i < matrix.GetLength(1); i++)
				{
					if (matrix[i, j] < lowest) lowest = matrix[i, j];
				}

				// If already reduced, move on.
				if (lowest == 0 || Double.IsPositiveInfinity(lowest)) continue;

				// Reduce each cell in the column by the lowest value.
				for (int i = 0; i < matrix.GetLength(1); i++)
				{
					matrix[i, j] -= lowest;
				}

				bound += lowest;
			}

			return bound;
		}

		protected double Reduce()
		{
			return ReduceRows() + ReduceColumns();
		}
		
		public Tuple<int, int> GetRecommendedIJ()
		{
			int lastGoodRow = -1;
			// Consider each row.
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				// Find a 0
				for (int j = 1; j < matrix.GetLength(1); j++)
				{
					if (matrix[i, j] == 0) return new Tuple<int, int>(i, j);
				}
				if (matrix [i, 0] == 0)
					lastGoodRow = i;
			}

			//return null;	
			return lastGoodRow == -1 ? null : new Tuple<int, int>(lastGoodRow, 0);
		}
		
		public CostMatrix IncludeEdge(int i, int j)
		{
			CostMatrix copy = this.Clone();

			// Set all cells in the given column to infinity.
			for (int _i = 0; _i < copy.matrix.GetLength(0); _i++) {
				copy.matrix[_i, j] = Double.PositiveInfinity;
			}

			// Set all cells in the given row to infinity.
			for (int _j = 0; _j < copy.matrix.GetLength(1); _j++) {
				copy.matrix[i, _j] = Double.PositiveInfinity;
			}

			// Don't go back to the edge we just left
			copy.matrix[j, i] = Double.PositiveInfinity;

			copy.bound += copy.Reduce();

			return copy;
		}
		
		public CostMatrix ExcludeEdge(int i, int j)
		{
			CostMatrix copy = this.Clone();
			copy.matrix[i, j] = Double.PositiveInfinity;
			copy.bound += copy.Reduce();
			return copy;
		}
		
		public CostMatrix Clone()
 		{
 			double[,] _matrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
 			Array.Copy(matrix, _matrix, matrix.Length);
			CostMatrix newCostMatrix = new CostMatrix (_matrix);
			newCostMatrix.bound = this.bound;
			return newCostMatrix;
 		}
	}
}

