import React, { useState } from 'react';
import { TextField, Button, Typography, Card, CardContent, Grid, IconButton, Box } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import { useNavigate } from 'react-router-dom';
import PollApiService from '../services/PollApiService';

const CreatePoll: React.FC = () => {
    const pollService = new PollApiService('https://localhost:7262/api');

    const [pollTitle, setPollTitle] = useState<string>('');
    const [options, setOptions] = useState<string[]>(['']);
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

        pollService.createPoll({ question: pollTitle, options: options }).then(() => {
            // Simulate poll creation logic
            alert(`Poll created: ${pollTitle}\nOptions: ${options.join(', ')}`);
            // alert('Poll created successfully.');
            // Redirect to the polls list or other page
            navigate('/');
        });
    };

    return (
        <div className="container mt-5">
            <Typography variant="h4" align="center" gutterBottom>
                Create a New Poll
            </Typography>
            <Card sx={{ boxShadow: 3, backgroundColor: '#FFEBEE' }}>
                <CardContent>
                    <Grid container spacing={2}>
                        {/* Poll Title */}
                        <Grid item xs={12}>
                            <TextField
                                label="Poll Title"
                                variant="outlined"
                                fullWidth
                                value={pollTitle}
                                onChange={(e) => setPollTitle(e.target.value)}
                            />
                        </Grid>

                        {/* Options */}
                        {options.map((option, index) => (
                            <Grid item xs={12} key={index}>
                                <Box display="flex" alignItems="center">
                                    <TextField
                                        label={`Option ${index + 1}`}
                                        variant="outlined"
                                        fullWidth
                                        value={option}
                                        onChange={(e) => handleOptionChange(index, e.target.value)}
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

                        {/* Add Option Button */}
                        <Grid item xs={12}>
                            <Button
                                variant="outlined"
                                color="primary"
                                fullWidth
                                onClick={handleAddOption}
                                startIcon={<AddIcon />}
                            >
                                Add Option
                            </Button>
                        </Grid>

                        {/* Submit Button */}
                        <Grid item xs={12}>
                            <Button
                                variant="contained"
                                color="secondary"
                                fullWidth
                                onClick={handleSubmit}
                                sx={{ marginTop: 2 }}
                            >
                                Create Poll
                            </Button>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>
        </div>
    );
};

export default CreatePoll;
