import React, { useState } from 'react';
import { TextField, Button, Typography, Grid, IconButton, Box, Container, Paper } from '@mui/material';
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import { useNavigate } from 'react-router-dom';
import PollApiService from '../services/PollApiService';
import dayjs from 'dayjs';

const CreatePoll: React.FC = () => {
    const pollService = new PollApiService('https://localhost:7262/api');

    const [pollTitle, setPollTitle] = useState<string>('');
    const [options, setOptions] = useState<string[]>(['']);
    const [expirationDate, setExpirationDate] = useState<dayjs.Dayjs>(dayjs().add(3, 'hour'));
    const navigate = useNavigate();

    const handleAddOption = () => {
        setOptions([...options, '']);
    };

    const handleRemoveOption = (index: number) => {
        const updatedOptions = options.filter((_, i) => i !== index);
        setOptions(updatedOptions);
    };

    const handleOptionChange = (index: number, value: string) => {
        const updatedOptions = options.map((option, i) => (i === index ? value : option));
        setOptions(updatedOptions);
    };

    const handleSubmit = () => {
        if (pollTitle.trim() === '' || options.some((option) => option.trim() === '')) {
            alert('Please fill in all fields.');
            return;
        }

        pollService.createPoll({ question: pollTitle, options: options, expirationDate: expirationDate.toISOString() }).then(() => {
            alert(`Poll created successfully!`);
            navigate('/');
        });
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Container maxWidth="sm" sx={{ mt: 5 }}>
                <Paper elevation={3} sx={{ padding: 4, borderRadius: 3, backgroundColor: '#FAFAFA' }}>
                    <Typography variant="h4" align="center" gutterBottom sx={{ fontWeight: 'bold', color: '#333' }}>
                        Create a New Poll
                    </Typography>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <TextField
                                label="Poll Title"
                                variant="outlined"
                                fullWidth
                                value={pollTitle}
                                onChange={(e) => setPollTitle(e.target.value)}
                                sx={{ backgroundColor: 'white', borderRadius: 1 }}
                            />
                        </Grid>

                        {options.map((option, index) => (
                            <Grid item xs={12} key={index}>
                                <Box display="flex" alignItems="center">
                                    <TextField
                                        label={`Option ${index + 1}`}
                                        variant="outlined"
                                        fullWidth
                                        value={option}
                                        onChange={(e) => handleOptionChange(index, e.target.value)}
                                        sx={{ backgroundColor: 'white', borderRadius: 1 }}
                                    />
                                    <IconButton
                                        color="error"
                                        onClick={() => handleRemoveOption(index)}
                                        sx={{ marginLeft: 1 }}
                                        disabled={options.length === 1}
                                    >
                                        <RemoveIcon />
                                    </IconButton>
                                </Box>
                            </Grid>
                        ))}

                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                color="primary"
                                fullWidth
                                onClick={handleAddOption}
                                startIcon={<AddIcon />}
                                sx={{ borderRadius: 2 }}
                            >
                                Add Option
                            </Button>
                        </Grid>

                        <Grid item xs={12}>
                            <DateTimePicker
                                label="Expiration Date"
                                value={expirationDate}
                                onChange={(newValue) => newValue && setExpirationDate(newValue)}
                                sx={{ width: '100%', backgroundColor: 'white', borderRadius: 1 }}
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <Button
                                variant="contained"
                                color="primary"
                                fullWidth
                                onClick={handleSubmit}
                                sx={{ marginTop: 2, borderRadius: 2 }}
                            >
                                Create Poll
                            </Button>
                        </Grid>
                    </Grid>
                </Paper>
            </Container>
        </LocalizationProvider>
    );
};

export default CreatePoll;
