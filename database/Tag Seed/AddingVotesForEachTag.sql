--chatgpt generated to add 1 vote for each tag
INSERT INTO movie_plot_tag_votes (
    imdb_id,
    plot_tag_id,
    user_id,
    vote,
    created_at
)
SELECT
    mpt.imdb_id,
    mpt.plot_tag_id,
    1,          -- replace with actual user_id
    1,          -- +1 vote
    NOW()
FROM movie_plot_tags mpt
WHERE mpt.imdb_id = 'tt26743210';