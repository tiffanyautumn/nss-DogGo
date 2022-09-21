SELECT W.Id, W.[Name], W.ImageUrl, W.NeighborhoodId, N.Name AS Neighborhood, N.Id AS NId
FROM Walker W
 LEFT JOIN Neighborhood N ON N.Id = W.NeighborhoodId
