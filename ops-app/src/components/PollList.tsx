import React, { useState, useEffect } from 'react';
import { Card, CardContent, Button, Typography, Grid, Box } from '@mui/material';
import PollIcon from '@mui/icons-material/Poll';
import BarChartIcon from '@mui/icons-material/BarChart';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import EventIcon from '@mui/icons-material/Event';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { useNavigate } from 'react-router-dom';
import { Poll } from '../models/Poll';
import PollApiService from '../services/PollApiService';
import { SpeedDial, SpeedDialAction, SpeedDialIcon } from '@mui/material';
import { subscribeToPollUpdates } from '../services/signalRService';

const PollList: React.FC = () => {
    const pollService = new PollApiService();
    const navigate = useNavigate();
    const [polls, setPolls] = useState<Poll[]>([]);
    //const [pollUpdates, setPollUpdates] = useState<number | null>(null);

    useEffect(() => {
        subscribeToPollUpdates((pollId: number) => {
            console.log('Received poll update:', pollId);
            //setPollUpdates(pollId);
            pollService.getPolls().then((fetchedPolls) => {
                setPolls(fetchedPolls);
            }).catch(error => {
                console.error('Error fetching polls:', error);
            });
        });

        pollService.getPolls().then((fetchedPolls) => {
            setPolls(fetchedPolls);
        }).catch(error => {
            console.error('Error fetching polls:', error);
        });

    }, []);

    const handleVote = (pollId: number) => navigate(`/poll/${pollId}`);
    const handleViewResults = (pollId: number) => navigate(`/poll/${pollId}/results`);
    const handleEdit = (pollId: number) => navigate(`/edit-poll/${pollId}`);
    const handleDelete = (pollId: number) => {
        // pollService.deletePoll(pollId).then(() => {
        //     setPolls(polls.filter(p => p.id !== pollId));
        // }).catch(error => console.error('Error deleting poll:', error));
    };

    return (
        <div className="container mt-5">
            <Typography variant="h4" align="center" gutterBottom>
                Polls
            </Typography>
            <Grid container spacing={3} justifyContent="center">
                {polls.map((poll: Poll) => (
                    <Grid item key={poll.id} xs={12} sm={6} md={4}>
                        <Card sx={{ boxShadow: 3, backgroundColor: '#FFEBEE', minHeight: 250, display: 'flex', flexDirection: 'column', position: 'relative' }}>
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
                                <Typography variant="body2" color="error" gutterBottom>
                                    <EventIcon sx={{ verticalAlign: 'middle', marginRight: 1 }} />
                                    {poll.expirationDate
                                        ? `Expires on: ${new Date(poll.expirationDate).toLocaleDateString()} ${new Date(poll.expirationDate).toLocaleTimeString()}`
                                        : 'No expiration'}
                                </Typography>
                            </CardContent>

                            <Box display="flex" justifyContent="space-between" p={2}>
                                {!poll.hasVoted && new Date(poll.expirationDate!) > new Date() && (
                                    <Button variant="contained" color="primary" onClick={() => handleVote(poll.id)} startIcon={<CheckCircleIcon />}>
                                        Vote
                                    </Button>
                                )}
                                {poll.hasVoted && (
                                    <Button variant="contained" color="secondary" onClick={() => handleViewResults(poll.id)} startIcon={<BarChartIcon />}>
                                        View Results
                                    </Button>
                                )}
                            </Box>

                            {/* Floating Action Menu - Moved to Bottom Right */}
                            <SpeedDial
                                ariaLabel="Poll actions"
                                sx={{ position: 'absolute', bottom: 16, right: 16 }}
                                icon={<SpeedDialIcon />}
                                direction="up"
                            >
                                <SpeedDialAction icon={<EditIcon />} tooltipTitle="Edit Poll" onClick={() => handleEdit(poll.id)} />
                                <SpeedDialAction icon={<DeleteIcon />} tooltipTitle="Delete Poll" onClick={() => handleDelete(poll.id)} />
                            </SpeedDial>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </div>
    );
};

export default PollList;
