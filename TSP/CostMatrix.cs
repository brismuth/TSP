using System;

namespace TSP
{
	class CostMatrix
	{
		private double[,] matrix;

		public CostMatrix(City[] cities)
		{
			matrix = new double[cities.Length,cities.Length];

			for (int i = 0; i < cities.Length; i++)
			{
				for (int j = 0; j < cities.Length; j++)
				{
					if (i == j) matrix[i,j] = Double.PositiveInfinity;
					matrix[i,j] = cities[i].costToGetTo(cities[j]);
				}
			}
		}
		
		public CostMatrix(double[,] matrix)
		{
			this.matrix = matrix;
		}

		private double ReduceRows()
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


		private double ReduceColumns()
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

		private double Reduce()
		{
			return ReduceRows() + ReduceColumns();
		}

		private void IncludeEdge(int i, int j)
		{
			// Set all cells in the given column to infinity.
			for (int _i = 0; _i < matrix.GetLength(0); _i++) {
				matrix[_i, j] = Double.PositiveInfinity;
			}

			// Set all cells in the given row to infinity.
			for (int _j = 0; _j < matrix.GetLength(1); _j++) {
				matrix[i, _j] = Double.PositiveInfinity;
			}
		}

		private void ExcludeEdge(int i, int j)
		{
			matrix[i, j] = Double.PositiveInfinity;
		}
		
		public CostMatrix Clone()
 		{
 			double[,] _matrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
 			matrix.CopyTo(_matrix, 0);
 			return new CostMatrix(_matrix);
 		}
	}
}

