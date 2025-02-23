import React, { useState, useEffect } from 'react';
import {
    Button, Card, CardContent, Typography, Radio, RadioGroup,
    FormControlLabel, FormControl, FormLabel
} from '@mui/material';
import { useParams, useNavigate, Link } from 'react-router-dom';
import PollApiService from '../services/PollApiService';
import { Poll } from '../models/Poll';

const VotePoll: React.FC = () => {
    const pollService = new PollApiService('https://localhost:7262/api');

    const { pollId } = useParams<{ pollId: string }>();
    const navigate = useNavigate(); // Hook for navigating

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
        <div className="container mt-5">
            <Typography variant="h4" align="center" gutterBottom>
                Vote for {poll?.question}
            </Typography>
            <Card sx={{ boxShadow: 3, backgroundColor: '#FFEBEE' }}>
                <CardContent>
                    <FormControl component="fieldset">
                        <FormLabel component="legend">Select an option:</FormLabel>
                        <RadioGroup
                            value={selectedOption}
                            onChange={(e) => setSelectedOption(e.target.value)}
                        >
                            {poll?.options.map((option, index) => (
                                <FormControlLabel
                                    key={index}
                                    value={option.id}
                                    control={<Radio />}
                                    label={option.optionText}
                                />
                            ))}
                        </RadioGroup>
                    </FormControl>
                    <Button
                        variant="contained"
                        color="primary"
                        fullWidth
                        onClick={handleVote}
                        disabled={!selectedOption}
                        sx={{ mt: 2 }}
                    >
                        Vote
                    </Button>
                </CardContent>
            </Card>
            <div className="text-center mt-4">
                <Button
                    component={Link}
                    to={`/poll/${pollId}/results`}
                    variant="outlined"
                    color="secondary"
                    sx={{ mr: 2 }}
                >
                    View Results
                </Button>
                <Button
                    variant="outlined"
                    color="error"
                    onClick={() => navigate('/')}
                >
                    Cancel / Back
                </Button>
            </div>
        </div>
    );
};

export default VotePoll;
