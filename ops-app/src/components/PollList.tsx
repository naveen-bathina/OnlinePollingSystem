import React, { useState, useEffect } from 'react';
import { Card, CardContent, Button, Typography, Grid, Box } from '@mui/material';
import PollIcon from '@mui/icons-material/Poll';
import BarChartIcon from '@mui/icons-material/BarChart';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import HighlightOffIcon from '@mui/icons-material/HighlightOff';
import { useNavigate } from 'react-router-dom';
import { Poll } from '../models/Poll';
import PollApiService from '../services/PollApiService';

const PollList: React.FC = () => {
    const pollService = new PollApiService('https://localhost:7262/api');

    const navigate = useNavigate();
    const [polls, setPolls] = useState<Poll[]>([]);

    useEffect(() => {
        pollService.getPolls().then((fetchedPolls) => {
            setPolls(fetchedPolls);
        }).catch(error => {
            console.error('Error fetching polls:', error);
        });
    }, []);

    const handleVote = (pollId: number) => {
        navigate(`/poll/${pollId}`);
    };

    const handleViewResults = (pollId: number) => {
        navigate(`/poll/${pollId}/results`);
    };

    const handleReset = (pollId: number) => {

        pollService.resetPoll(pollId).then(() => {
            pollService.getPolls().then((fetchedPolls) => {
                setPolls(fetchedPolls);
            });
        }).catch(error => {
            console.error('Error resetting poll:', error);
        });
    };

    return (
        <div className="container mt-5">
            <Typography variant="h4" align="center" gutterBottom>
                Polls
            </Typography>
            <Grid container spacing={3} justifyContent="center">
                {polls.map((poll: Poll) => (
                    <Grid item key={poll.id} xs={12} sm={6} md={4}>
                        <Card sx={{ boxShadow: 3, backgroundColor: '#FFEBEE', minHeight: 250, display: 'flex', flexDirection: 'column' }}>
                            <CardContent sx={{ flexGrow: 1 }}>
                                <Typography variant="h6" component="div" gutterBottom>
                                    <PollIcon sx={{ verticalAlign: 'middle', marginRight: 1 }} />
                                    {poll.question}
                                </Typography>
                                <Typography variant="body2" color="text.secondary" gutterBottom>
                                    {poll.options.length} {poll.options.length > 1 ? 'Options' : 'Option'}
                                </Typography>
                                <Typography variant="body2" color="text.secondary" gutterBottom>
                                    {poll.options.reduce((sum, option) => sum + option.voteCount, 0)} {poll.options.reduce((sum, option) => sum + option.voteCount, 0) > 1 ? 'Votes' : 'Vote'}
                                </Typography>
                            </CardContent>
                            <Box display="flex" justifyContent="space-between" p={2}>

                                {!poll.hasVoted && (
                                    <Button
                                        variant="contained"
                                        color="primary"
                                        onClick={() => handleVote(poll.id)}
                                        startIcon={<CheckCircleIcon />}
                                    >
                                        Vote
                                    </Button>
                                )}

                                {poll.hasVoted && (
                                    <Button
                                        variant="contained"
                                        color="secondary"
                                        onClick={() => handleViewResults(poll.id)}
                                        startIcon={<BarChartIcon />}
                                    >
                                        View Results
                                    </Button>
                                )}

                                {poll.hasVoted && (<Button
                                    variant="outlined"
                                    color="error"
                                    onClick={() => handleReset(poll.id)}
                                    startIcon={<HighlightOffIcon />}
                                >
                                    Reset
                                </Button>)}
                            </Box>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </div>
    );
};

export default PollList;
