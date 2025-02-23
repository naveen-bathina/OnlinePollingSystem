import React, { useState, useEffect } from 'react';
import {
    Button, Typography, Radio, RadioGroup,
    FormControlLabel, FormControl, FormLabel, Container, Paper, Box
} from '@mui/material';
import { useParams, useNavigate, Link } from 'react-router-dom';
import PollApiService from '../services/PollApiService';
import { Poll } from '../models/Poll';

const VotePoll: React.FC = () => {
    const pollService = new PollApiService('https://localhost:7262/api');

    const { pollId } = useParams<{ pollId: string }>();
    const navigate = useNavigate();

    const [selectedOption, setSelectedOption] = useState<string>('');
    const [poll, setPoll] = useState<Poll>();

    useEffect(() => {
        const fetchPoll = async () => {
            try {
                const fetchedPoll = await pollService.getPoll(Number(pollId));
                setPoll(fetchedPoll);
            } catch (error) {
                console.error('Error fetching poll:', error);
            }
        };

        fetchPoll();
    }, []);

    const handleVote = async () => {
        if (selectedOption) {
            await pollService.votePoll(Number(pollId), Number(selectedOption)).then(() => {
                navigate(`/poll/${pollId}/results`);
            });
        }
    };

    return (
        <Container maxWidth="sm" sx={{ mt: 5 }}>
            <Paper elevation={4} sx={{ padding: 4, borderRadius: 3, backgroundColor: '#FAFAFA' }}>
                <Typography variant="h4" align="center" gutterBottom sx={{ fontWeight: 'bold', color: '#333' }}>
                    Vote for: {poll?.question}
                </Typography>
                <Box mt={2}>
                    <FormControl component="fieldset" fullWidth>
                        <FormLabel component="legend" sx={{ fontWeight: 'bold', color: '#555' }}>Select an option:</FormLabel>
                        <RadioGroup
                            value={selectedOption}
                            onChange={(e) => setSelectedOption(e.target.value)}
                        >
                            {poll?.options.map((option, index) => (
                                <FormControlLabel
                                    key={index}
                                    value={option.id}
                                    control={<Radio color="primary" />}
                                    label={option.optionText}
                                    sx={{ backgroundColor: 'white', borderRadius: 1, padding: 1, mt: 1 }}
                                />
                            ))}
                        </RadioGroup>
                    </FormControl>
                </Box>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={handleVote}
                    disabled={!selectedOption}
                    sx={{ mt: 3, borderRadius: 2 }}
                >
                    Submit Vote
                </Button>
                <Box mt={3} display="flex" justifyContent="space-between">
                    <Button
                        component={Link}
                        to={`/poll/${pollId}/results`}
                        variant="outlined"
                        color="secondary"
                        sx={{ borderRadius: 2 }}
                    >
                        View Results
                    </Button>
                    <Button
                        variant="outlined"
                        color="error"
                        onClick={() => navigate('/')}
                        sx={{ borderRadius: 2 }}
                    >
                        Cancel / Back
                    </Button>
                </Box>
            </Paper>
        </Container>
    );
};

export default VotePoll;
