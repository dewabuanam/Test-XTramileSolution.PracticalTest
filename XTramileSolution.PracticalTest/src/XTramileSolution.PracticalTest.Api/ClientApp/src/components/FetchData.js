import React, { useState, useEffect } from 'react';
import { Container, Form, FormGroup, Table, Spinner } from 'react-bootstrap';

const FetchData = () => {
  const [countries, setCountries] = useState([]);
  const [cities, setCities] = useState([]);
  const [selectedCountry, setSelectedCountry] = useState('');
  const [selectedCity, setSelectedCity] = useState('');
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [loadingCities, setLoadingCities] = useState(false);

  useEffect(() => {
    fetchCountries();
  }, []);

  const fetchCountries = async () => {
    try {
      const response = await fetch(`${process.env.REACT_APP_BASE_API}/countries`);
      const data = await response.json();
      setCountries(data);
    } catch (error) {
      console.error('Error fetching countries:', error);
    }
  };

  const fetchCities = async (countryId) => {
    setLoadingCities(true);
    try {
      const response = await fetch(`${process.env.REACT_APP_BASE_API}/cities/${countryId}`);
      const data = await response.json();
      setCities(data);
    } catch (error) {
      console.error('Error fetching cities:', error);
    }
    setLoadingCities(false);
  };

  const fetchWeather = async (cityId) => {
    setLoading(true);
    try {
      const response = await fetch(`${process.env.REACT_APP_BASE_API}/weather/${cityId}`);
      const data = await response.json();
      setWeather({
        location: `${data.weatherLocationResponse.city}, ${data.weatherLocationResponse.country}`,
        time: new Date(data.time.localTime).toUTCString() + " (" + data.time.offset + ')',
        wind: `${data.weatherWindResponse.speed} km/h`,
        visibility: `${data.visibility} m`,
        sky: data.skyConditions.map(condition => condition.description).join(', '),
        temperatureC: data.weatherTemperatureResponse.celsius.toFixed(1),
        temperatureF: data.weatherTemperatureResponse.fahrenheit.toFixed(1),
        dewPoint: `${data.weatherTemperatureResponse.dewPoint.toFixed(1)}°C`,
        humidity: `${data.humidity}%`,
        pressure: `${data.pressure} hPa`
      });
    } catch (error) {
      console.error('Error fetching weather:', error);
    }
    setLoading(false);
  };

  return (
      <Container>
        <h1>Xtramile Solutions - Servicing you well above and beyond</h1>
        <Form>
          <FormGroup>
            <Form.Label htmlFor="country">Country</Form.Label>
            <Form.Control as="select" id="country" onChange={(e) => {
              const countryId = e.target.value;
              setSelectedCountry(countryId);
              fetchCities(countryId);
            }}>
              <option value="">Select a country</option>
              {countries.map(country => (
                  <option key={country.id} value={country.id}>{country.name}</option>
              ))}
            </Form.Control>
          </FormGroup>
          <FormGroup style={{ paddingTop: '10px', position: 'relative' }}>
            <Form.Label htmlFor="city">City</Form.Label>
            {loadingCities && (
                <div style={{ position: 'absolute', left: '50%', top: '50%', transform: 'translate(-50%, -50%)' }}>
                  <Spinner animation="border" />
                </div>
            )}
            <Form.Control as="select" id="city" disabled={!selectedCountry || loadingCities} onChange={(e) => {
              setSelectedCity(e.target.value);
              fetchWeather(e.target.value);
            }}>
              <option value="">Select a city</option>
              {cities.map(city => (
                  <option key={city.id} value={city.id}>{city.name}</option>
              ))}
            </Form.Control>
          </FormGroup>
        </Form>

        {loading && <Spinner animation="border" style={{ display: 'block', margin: '20px auto' }} />}

        {weather && (
            <Table striped bordered hover className="mt-3">
              <thead>
              <tr>
                <th>Location</th>
                <th>Time</th>
                <th>Wind</th>
                <th>Visibility</th>
                <th>Sky Conditions</th>
                <th>Temperature (C)</th>
                <th>Temperature (F)</th>
                <th>Dew Point</th>
                <th>Humidity</th>
                <th>Pressure</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <td>{weather.location}</td>
                <td>{weather.time}</td>
                <td>{weather.wind}</td>
                <td>{weather.visibility}</td>
                <td>{weather.sky}</td>
                <td>{weather.temperatureC}</td>
                <td>{weather.temperatureF}</td>
                <td>{weather.dewPoint}</td>
                <td>{weather.humidity}</td>
                <td>{weather.pressure}</td>
              </tr>
              </tbody>
            </Table>
        )}
      </Container>
  );
};

export default FetchData;