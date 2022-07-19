<script>
  import { Link } from "yrv";
  import Container from "../components/Container.svelte";
  import StatsContainer from "./StatsContainer.svelte";
  import Commit from "./commitGrid/Commit.svelte";
  import CommitGrid from "./commitGrid/CommitGrid.svelte";
  import Row from "./teamRow/Row.svelte";
  import Time from "./Time.svelte";
</script>

<style>
  .pipeline {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr 1fr;
  }
  h4 {
    text-align: center;
  }
 
</style>

<Container>
  <h1>Product</h1>
</Container>
<Container>
  <div class="pipeline">
    <Row>
      <div slot="first"></div>
      <h4 slot="second">Commit</h4>
      <h4 slot="third">Build</h4>
      <h4 slot="fourth">Dev</h4>
      <h4 slot="fifth">Staging</h4>
      <h4 slot="sixth">Simulation</h4>
      <h4 slot="seventh">Production</h4>
    </Row>
    <Row highlight>
      <h4 slot="first">Geckos</h4>
      <div slot="second">
        <Time time="00:21"/>
        <StatsContainer time={10} />
        <CommitGrid>
          <Commit />
          <Commit bad commitcount={11} />
          <Commit />
          <Commit />
          <Commit />
          <Commit />
          <Commit commitcount={7} />
          <Commit commitcount={7} />
          <Commit />
          <Commit />
          <Commit />
          <Commit />
          <Commit commitcount={100} />
          <Commit />
          <Commit />
        </CommitGrid>
      </div>
      <div slot="third"></div>
      <div slot="fourth">
        <Time time="00:00:02"/>
        <StatsContainer />
        <CommitGrid>
          <Commit />
          <Commit bad commitcount={11} />
          <Commit />
          <Commit />
          <Commit />
          <Commit />
          <Commit commitcount={7} />
          <Commit commitcount={7} />
          <Commit />
        </CommitGrid>
      </div>
      <div slot="fifth"></div>
      <div slot="sixth"></div>
      <div slot="seventh"></div>
    </Row>
    <Row>
      <h4 slot="first">Skeles</h4>
      <div slot="second">
        
      </div>
      <div slot="third">
        <Time time="1:00:21"/>
        <StatsContainer time={75} />
        <CommitGrid>
          <Commit />
          <Commit bad commitcount={11} />
          <Commit />
          <Commit />
          <Commit />
          <Commit />
          <Commit commitcount={7} />
          <Commit commitcount={7} />
          <Commit />
        </CommitGrid>
      </div>
      <div slot="fourth"></div>
      <div slot="fifth"></div>
      <div slot="sixth">
        <Time time="01:00"/>
        <StatsContainer />
        <CommitGrid>
          <Commit />
          <Commit bad commitcount={11} />
          <Commit />
          <Commit />
          <Commit />
          <Commit />
          <Commit commitcount={7} />
          <Commit commitcount={7} />
          <Commit />
        </CommitGrid>
      </div>
      <div slot="seventh"></div>
    </Row>
  </div>
</Container>
