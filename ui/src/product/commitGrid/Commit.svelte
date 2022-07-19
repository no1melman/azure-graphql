<script>
  export let commitcount = 0
  export let bad = false

  let commitClass = commitcount <= 1 ? 'single-commit' : commitcount <= 10 ? 'tens-commit' : 'hundred-commit';
  let badClass = bad ? 'bad-commit' : '';
</script>

<style>
  .acommit {
    margin: 1px;
    background-color: #669900;
  }

  .bad-commit {
    background-color: #b22222;
  }

  .single-commit {
    grid-column: span 1;
    grid-row: span 1;
  }

  .tens-commit {
    grid-column: span 2;
    grid-row: span 1;
  }

  .hundred-commit {
    grid-column: span 2;
    grid-row: span 2;
  }
</style>

<div class="acommit {[badClass, commitClass].join(' ')}"></div>