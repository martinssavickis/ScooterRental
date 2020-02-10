# ScooterRental

Not quite sure what was the expected design, but decided to go for classic n-layer and probably overdid it (this would look great with event-sourcing)

Created just the interfaces for DAL (decided not to write in-memory mocks)

Couple of business questions found during development:
1. How should the rental price be calculated for seconds (went with whole minutes)
2. If yearly income is being calculated and rental spans multiple years, how should it be calculated? Went with start date, EndRent description hints about expected implementation?
