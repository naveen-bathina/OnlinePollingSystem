import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import Header from './components/Header'; // Import Header
import PollList from './components/PollList';
import CreatePoll from './components/CreatePoll';
import VotePoll from './components/VotePoll';
import Results from './components/Results';
import DeviceApiService from './services/DeviceApiService';
import cookies from 'js-cookie';

// Create the Orange MUI theme
const theme = createTheme({
  palette: {
    primary: {
      main: '#FF5722', // Orange color for primary
    },
    secondary: {
      main: '#FF9800', // A lighter shade of orange for secondary actions
    },
    background: {
      default: '#FBE9E7', // Light orange background color for the application
    },
    text: {
      primary: '#212121', // Dark text for readability
      secondary: '#757575', // Lighter text for secondary content
    },
  },
  typography: {
    fontFamily: 'Roboto, sans-serif', // Default font family for the app
    h6: {
      fontWeight: 'bold', // Ensure h6 has bold font for headers
    },
  },
});

const App: React.FC = () => {

  React.useEffect(() => {
    const deviceService = new DeviceApiService('https://localhost:7262/api');
    const fetchDeviceId = async () => {
      try {
        const existingDeviceId = cookies.get('deviceId');
        if (existingDeviceId) {
          return;
        }

        const deviceData = await deviceService.getDeviceId();
        const deviceId = deviceData.deviceId;
        if (deviceId) {
          cookies.set('deviceId', deviceId, { expires: 1 }); // Set cookie for 1 year
        }
      } catch (error) {
        console.error('Error fetching device ID:', error);
      }
    };

    fetchDeviceId();
  }, []);


  return (
    <ThemeProvider theme={theme}>
      <Router>
        <Header />
        <div>
          <Routes>
            <Route path="/" element={<PollList />} />
            <Route path="/create" element={<CreatePoll />} />
            <Route path="/poll/:pollId" element={<VotePoll />} />
            <Route path='/poll/:pollId/results' element={<Results />} />
          </Routes>
        </div>
      </Router>
    </ThemeProvider>
  );
};

export default App;
