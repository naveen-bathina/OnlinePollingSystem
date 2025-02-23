import React from 'react';
import { AppBar, Toolbar, Typography, Button, Container } from '@mui/material';
import { Link } from 'react-router-dom';
import HomeIcon from '@mui/icons-material/Home';
import AddCircleIcon from '@mui/icons-material/AddCircle';

const Header: React.FC = () => {
    return (
        <AppBar position="sticky" sx={{ backgroundColor: '#FF5722' }}> {/* Apply primary color */}
            <Toolbar>
                <Container maxWidth="lg" sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    {/* Application Title */}
                    <Typography variant="h6" sx={{ fontWeight: 'bold' }}>
                        Online Polling System
                    </Typography>

                    {/* Navigation Links with Icons */}
                    <div>
                        <Button
                            component={Link}
                            to="/"
                            sx={{ color: 'white', marginRight: 2, fontWeight: 'bold' }}
                            startIcon={<HomeIcon />}
                        >
                            Home
                        </Button>
                        <Button
                            component={Link}
                            to="/create"
                            sx={{ color: 'white', fontWeight: 'bold' }}
                            startIcon={<AddCircleIcon />}
                        >
                            Create Poll
                        </Button>
                    </div>
                </Container>
            </Toolbar>
        </AppBar>
    );
};

export default Header;
