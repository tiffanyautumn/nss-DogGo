SELECT O.Id, O.[Name], O.Email, O.Address, O.NeighborhoodId, O.Phone, D.Name, D.Breed, D.ImageUrl
FROM Owner O
LEFT JOIN Dog D on D.OwnerId = O.Id